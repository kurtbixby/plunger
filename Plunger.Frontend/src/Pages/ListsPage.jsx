import {useParams} from "react-router-dom";
import {useContext} from "react";
import CurrentUserContext from "../CurrentUserContext";

function ListsPage() {
    const [currentUser, ] = useContext(CurrentUserContext);
    const { userName } = useParams();
    
    return (
        <section>
            <div>
                <h2>{userName + 's Lists'}</h2>
            </div>
        </section>
    );
}

export default ListsPage;