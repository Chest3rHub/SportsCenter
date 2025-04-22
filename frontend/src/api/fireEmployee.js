import API_URL from "../appConfig";

export default async function fireEmployee(id, token){
    return fetch(`${API_URL}/Employees/dismiss-employee`, { 
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          dismissedEmployeeId: id,
          dismissalDate: new Date(new Date().getTime() - new Date().getTimezoneOffset() * 60000).toISOString().slice(0, 10),
        }),
        credentials: 'include',
      });
}