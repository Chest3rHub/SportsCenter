import API_URL from "../appConfig";

export default async function addNews(formData, token){
    return fetch(`${API_URL}/news/add-news`, { 
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          Title: formData.title,
          Content: formData.content,
          ValidFrom: formData.validFrom,
          ValidUntil: formData.validUntil,
        }),
      });
}