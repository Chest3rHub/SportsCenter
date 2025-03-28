import API_URL from "../appConfig";

export default async function getNews(offset){
    return fetch(`${API_URL}/News/Get-news?offset=${offset}`, {

        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });
}