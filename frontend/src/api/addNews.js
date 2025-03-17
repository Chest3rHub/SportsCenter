import API_URL from "../appConfig";

export default async function addNews(formData){
    return fetch(`${API_URL}/news/add-news`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          Title: formData.title,
          Content: formData.content,
          ValidFrom: formData.validFrom,
          ValidUntil: formData.validUntil,
        }),
      });
}