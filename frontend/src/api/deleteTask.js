import API_URL from "../appConfig";

const deleteTask = (token, taskId) => {
  return fetch(`${API_URL}/Employees/Delete-task`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({ taskId }),
    credentials: 'include'
  });
};

export default deleteTask;