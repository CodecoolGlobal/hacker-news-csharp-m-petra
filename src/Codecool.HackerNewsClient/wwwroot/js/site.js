const topNewsButton = document.querySelector("body > header > nav > div > div > ul > li:nth-child(2)");
const newestButton = document.querySelector("body > header > nav > div > div > ul > li:nth-child(3)");
const jobsButton = document.querySelector("body > header > nav > div > div > ul > li:nth-child(4)");
const previousButton = document.querySelector("body > div > main > button:nth-child(1)");
const nextButton = document.querySelector("body > div > main > button:nth-child(2)");
window.currentPage = 1;

async function createCards(event) {
    const url = event.currentTarget.dataset.url;
    previousButton.dataset.url = url;
    nextButton.dataset.url = url;
    loadCards(url);
}

async function loadCards(url) {
    const results = await fetchData(`/api/${url}?page=${window.currentPage}`);
    if (results === null) {
        return;
    }
    const inputField = document.getElementById('field');
    console.log(inputField);
    let cards = "";
    for (const row of results) {
        cards +=
            `
            <div class="card box show-card">
                <div><a href="${row['Url']}">${row['Title']}</a> </div>
                <div> ${row['Author']}</div>
                <div> ${row['TimeAgo']}</div>
                </div>
            `;
    }
    inputField.innerHTML = cards;
}

async function fetchData(route) {
    const response = await fetch(route);
    const data = await response.json();
    if (data.NewsList.length === 0) {
        return null;
    }
    return data.NewsList;
}

async function nextPage(event) {
    window.currentPage++;
    createCards(event);
}

async function previousPage(event) {
    if (window.currentPage === 1) {
        return;
    }
    window.currentPage--;
    createCards(event);
}

function init() {
    loadCards("top");
    topNewsButton.addEventListener("click", createCards);
    newestButton.addEventListener("click", createCards);
    jobsButton.addEventListener("click", createCards);
    previousButton.addEventListener("click", previousPage);
    nextButton.addEventListener("click", nextPage);
}

init()