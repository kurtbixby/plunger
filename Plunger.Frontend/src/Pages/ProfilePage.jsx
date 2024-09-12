import {useParams} from "react-router-dom";

function ProfilePage() {
    const { userName } = useParams();
    
    return (
        <section>
            <div>
                <h2>{userName + 's Profile'}</h2>
            </div>
        </section>
    );
}

export default ProfilePage;