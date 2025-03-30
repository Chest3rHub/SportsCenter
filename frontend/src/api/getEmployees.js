import API_URL from "../appConfig";

export default async function getEmployees(token,offset){
    return fetch(`${API_URL}/Employees?offset=${offset}`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
}