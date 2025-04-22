import API_URL from "../appConfig";

export default async function addClientDiscount(formData, token){
    return fetch(`${API_URL}/Clients/add-client-discount`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          clientEmail: formData.clientEmail,
          activityDiscount: formData.activityDiscount,
          productDiscount: formData.productDiscount
        }),
        credentials: 'include',
      });
}