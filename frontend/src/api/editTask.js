import API_URL from "../appConfig";

const editTask = (taskData) => {
  return fetch(`${API_URL}/Employees/Edit-task`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(taskData),
    credentials: 'include'
  });
};

export default editTask;