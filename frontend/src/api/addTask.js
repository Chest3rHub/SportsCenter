import API_URL from "../appConfig";

const addTask = (taskData) => {
  return fetch(`${API_URL}/Employees/Add-task`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(taskData),
    credentials: 'include'
  });
};

export default addTask;