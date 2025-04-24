import API_URL from "../appConfig";

const editTask = (token, taskData) => {
  return fetch(`${API_URL}/Employees/Edit-task`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(taskData),
    credentials: 'include'
  });
};

export default editTask;