import API_URL from "../appConfig";

export default async function addClientDiscount(formData, token){
    return fetch(`${API_URL}/Clients/add-client-discount`, { 
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          clientEmail: formData.clientEmail,
          activityDiscount: formData.ActivityDiscount,
          productDiscount: formData.ProductDiscount
        }),
      });
}