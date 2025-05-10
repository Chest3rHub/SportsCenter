import API_URL from "../appConfig";

const signUpForActivity = (activityData) => {
  return fetch(`${API_URL}/SportActivities/sign-up-for-activity`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(activityData),
    credentials: 'include'
  });
};

export default signUpForActivity;