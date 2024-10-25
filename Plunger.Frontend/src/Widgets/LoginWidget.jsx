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

    const formData = objFromForm(e.target);
    
    // if (e.nativeEvent.submitter.name === "login") {
      await handleLogin(formData);
    // } else if (e.nativeEvent.submitter.name === "register") {
    //   await handleRegister(formData);
    // }

    // userDispatch({ type: "sendLoginRequest" });
    // var userDetails = await API.sendLoginRequest({
    //   identity: formData.username,
    //   password: formData.password,
    // });
    // userDispatch({
    //   type: "loginSucceeded",
    //   payload: { username: userDetails.userName, userId: userDetails.userId },
    // });

    // setUser({...user, loggedIn: true, userName: userDetails.userName, userId: userDetails.userId});
    console.log(userDetails);
  }
  
  async function handleLogin(formData) {
    userDispatch({ type: "sendLoginRequest" });
    const userDetails = await API.sendLoginRequest({
      identity: formData.username,
      password: formData.password,
    });
    userDispatch({
      type: "loginSucceeded",
      payload: { username: userDetails.userName, userId: userDetails.userId },
    });
  }
  
  // async function handleRegister(formData) {
  //   userDispatch({ type: "sendRegisterRequest" });
  //   const result = await API.sendNewUserRequest();
  //   await handleLogin(formData);
  // }

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
        <button type="submit" name="login">Login</button>
        {/*<button type="submit" name="register">Register</button>*/}
      </form>
    </div>
  );
}

export default LoginWidget;
