import API_URL from "../appConfig";

const deleteSportActivity = (sportActivityId) => {
  return fetch(`${API_URL}/SportActivities/remove-sport-activity`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ sportActivityId }),
    credentials: 'include'
  });
};

export default deleteSportActivity;