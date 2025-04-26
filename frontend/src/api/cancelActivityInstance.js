import API_URL from "../appConfig";

const cancelActivityInstance = (activityId, activityDate) => {
  return fetch(`${API_URL}/SportActivities/cancel-activity-instance`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ activityId, activityDate }),
    credentials: 'include'
  });
};

export default cancelActivityInstance;