import {useContext} from "react";
import CurrentUserContext from "./CurrentUserContext.js";
import LoginWidget from "./LoginWidget.jsx";

function TopNavigation() {
    const [user, ] = useContext(CurrentUserContext);
    
  return (
    <header>
        {user.loggedIn ? <h1>Time to clear your backlog {user}!</h1>
            : <LoginWidget />
        }
    </header>
  );
}

export default TopNavigation;
