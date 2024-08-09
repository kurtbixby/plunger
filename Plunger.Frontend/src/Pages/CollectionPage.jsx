import {useParams} from "react-router-dom";
import {useContext, useEffect, useState} from "react";
import CurrentUserContext from "../CurrentUserContext";
import APICalls from "../APICalls.js";

function CollectionPage() {
    const [currentUser] = useContext(CurrentUserContext);
    const { userName } = useParams();
    
    const [collectionResults, setCollectionResults] = useState([]);
    
    useEffect(() => {
        (async () => {
            const collectionPageResponse = await APICalls.sendGetCollectionRequest(userName);
            setCollectionResults(collectionPageResponse.games);
        })()
    }, [])
    
    return (
        <section>
            <div>
                <h2>{userName + 's Collection'}</h2>
            </div>
            <div>
                {collectionResults?.map(ci =>
                    <div>
                        <p>Time Added: {ci.timeAdded}</p>
                        <p>Time Acquired: {ci.timeAcquired}</p>
                        <p>Physicality: {ci.physicality}</p>
                        <p>Purchase Price: {ci.purchasePrice}</p>
                        <p>Game ID: {ci.game.id}</p>
                        <p>Game Name: {ci.game.name}</p>
                        <p>Platform ID: {ci.platform.id}</p>
                        <p>Platform Name: {ci.platform.name}</p>
                        <p>Region ID: {ci.region.id}</p>
                        <p>Region Name: {ci.region.name}</p>
                        <hr/>
                    </div>
                )}
            </div>
        </section>
    );
}

export default CollectionPage;