import API_URL from "../appConfig";

export default function cancelSignUpForActivity(instanceOfActivityId, selectedDate){
  return fetch(`${API_URL}/SportActivities/cancel-sign-up-for-activity`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ instanceOfActivityId, selectedDate }),
    credentials: 'include'
  });
};

