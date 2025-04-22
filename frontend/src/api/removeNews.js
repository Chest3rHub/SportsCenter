import API_URL from "../appConfig";

export default async function removeNews(id, token) {
    return fetch(`${API_URL}/News/Remove-news/?newsId=${id}`, {

        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
}