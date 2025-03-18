import API_URL from "../appConfig";

export default async function editNews(formData, token){
    return fetch(`${API_URL}/news/update-news/`, { 
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          NewsId: formData.id,
          Title: formData.title,
          Content: formData.content,
          ValidFrom: formData.validFrom,
          ValidUntil: formData.validUntil,
        }),
      });
}