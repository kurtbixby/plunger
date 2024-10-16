import { useState } from "react";
import { objFromForm } from "../Utils.js";
import API from "../APICalls.js";
import { useCurrentUser } from "../CurrentUserProvider.jsx";

function LoginWidget() {
  const { dispatch: userDispatch } = useCurrentUser();

  const [state, setState] = useState({
    username: "",
    password: "",
  });

  async function handleSubmit(e) {
    e.preventDefault();

    var formData = objFromForm(e.target);

    userDispatch({ type: "sendLoginRequest" });
    var userDetails = await API.sendLoginRequest({
      identity: formData.username,
      password: formData.password,
    });
    userDispatch({
      type: "loginSucceeded",
      payload: { username: userDetails.userName, userId: userDetails.userId },
    });

    // setUser({...user, loggedIn: true, userName: userDetails.userName, userId: userDetails.userId});
    console.log(userDetails);
  }

  function editValue(name, value) {
    setState({
      ...state,
      [name]: value,
    });
  }

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="username">Username:</label>
          <input
            name="username"
            type="text"
            value={state.username}
            onChange={(e) => editValue(e.target.name, e.target.value)}
          />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input
            name="password"
            type="password"
            value={state.password}
            onChange={(e) => editValue(e.target.name, e.target.value)}
          />
        </div>
        <button type="submit">Login</button>
      </form>
    </div>
  );
}

export default LoginWidget;
