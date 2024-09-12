import {Link} from "react-router-dom";
import LoginWidget from "./LoginWidget";
import {useCurrentUser} from "../CurrentUserProvider.jsx";

function TopNavigation() {
    // const [user, ] = useContext(CurrentUserContext);
    const { state: {isLoggedIn, isLoading, user} } = useCurrentUser();
    
  return (
    <header>
        <nav>
            <Link to={"/"}>Home</Link>
            {!isLoggedIn ? <LoginWidget/> :
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
            }
        </nav>
        {isLoggedIn && <h1>Time to clear your backlog {user.username}!</h1>}
    </header>
  );
}

export default TopNavigation;
