import API_URL from "../appConfig";

export default async function changeSomeonePassword(formData,id, token){
    return fetch(`${API_URL}/users/change-other-user-password`, { 
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            value: formData.newPassword,
            userId:id,
        }),
        credentials: 'include',
      });
}