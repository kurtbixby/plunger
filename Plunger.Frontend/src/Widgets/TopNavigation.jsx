import {useContext} from "react";
import {Link} from "react-router-dom";
import CurrentUserContext from "../CurrentUserContext";
import LoginWidget from "./LoginWidget";

function TopNavigation() {
    const [user, ] = useContext(CurrentUserContext);
    
  return (
    <header>
        {!user.loggedIn ? <LoginWidget/>
            : <>
                <h1>Time to clear your backlog {user.userName}!</h1>
                <nav>
                    <ul>
                        <li>
                            <Link to={"/" + user.userName}>My Profile</Link>
                        </li>
                        <li>
                            <Link to={"/" + user.userName + "/collection"}>Collection</Link>
                        </li>
                        <li>
                            <Link to={"/" + user.userName + "/lists"}>Lists</Link>
                        </li>
                    </ul>
                </nav>
            </>
        }
    </header>
  );
}

export default TopNavigation;
