import API_URL from "../appConfig";

const addTask = (token, taskData) => {
  return fetch(`${API_URL}/Employees/Self-Add-task`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(taskData),
    credentials: 'include'
  });
};

export default addTask;