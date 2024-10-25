import {useCurrentUser} from "../CurrentUserProvider.jsx";
import {useEffect} from "react";
import {useNavigate} from "react-router-dom";

function LogoutPage(props) {    
    const { state: { isLoggedIn }, dispatch: userDispatch } = useCurrentUser();
    const navigate = useNavigate();

    useEffect(() => {
        if (isLoggedIn) {
            logout();
        }
        navigate("/");
    }, []);
    
    function logout() {
        userDispatch({type: "logout"});
    }
    
    return <div>
        {isLoggedIn ? <p>Logging you out</p> : <p>Not logged in</p>}
    </div>
}

export default LogoutPage;