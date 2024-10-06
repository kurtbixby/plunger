import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import APICalls from "../APICalls.js";
import Enums from "../Enums.js";
import { dateFormat, formatCurrency } from "../Utils.js";
import CollectionTable from "../Widgets/CollectionTable.jsx";

function CollectionPage() {
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
            <section className={"flex justify-center"}>
                <CollectionTable collectionEntries={collectionResults}/>
            </section>
        </section>
    );
}

export default CollectionPage;