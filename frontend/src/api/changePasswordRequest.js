import API_URL from "../appConfig";

export default async function changePasswordRequest(formData, token){
    return fetch(`${API_URL}/users/change-password-yourself`, { 
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            value: formData.newPassword,
        }),
      });
}