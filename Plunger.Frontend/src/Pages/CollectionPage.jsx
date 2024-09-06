import {useParams} from "react-router-dom";
import {useContext, useEffect, useState} from "react";
import CurrentUserContext from "../CurrentUserContext";
import APICalls from "../APICalls.js";
import Enums from "../Enums.js";
import { dateFormat, formatCurrency } from "../Utils.js";

function CollectionPage() {
    const [currentUser, ] = useContext(CurrentUserContext);
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
                <div className={"basis-11/12"}>
                    <div className={"flex justify-center"}>
                        <p className={"basis-1/4"}>Game</p>
                        <p className={"basis-2/12"}>Platform</p>
                        <p className={"basis-1/12"}>Purchase Price</p>
                        <p className={"basis-1/12"}>Physicality</p>
                        <p className={"basis-1/12"}>Region</p>
                        <p className={"basis-1/12"}>Date Acquired</p>
                        <p className={"basis-1/12"}>Date Added</p>
                    </div>
                    {collectionResults?.map(ci =>
                        <div key={ci.id} >
                            <div className={"flex justify-center"}>
                                <p className={"basis-1/4"}>{ci.game.name}</p>
                                <p className={"basis-2/12"}>{ci.platform.name}</p>
                                <p className={"basis-1/12"}>{formatCurrency(ci.purchasePrice)}</p>
                                <p className={"basis-1/12"}>{Enums.Physicality[ci.physicality]}</p>
                                <p className={"basis-1/12"}>{ci.region.name}</p>
                                <p className={"basis-1/12"}>{dateFormat(ci.timeAcquired)}</p>
                                <p className={"basis-1/12"}>{dateFormat(ci.timeAdded)}</p>
                                {/*<p>Game ID: {ci.game.id}</p>*/}
                                {/*<p>Platform ID: {ci.platform.id}</p>*/}
                                {/*<p>Region ID: {ci.region.id}</p>*/}
                            </div>
                            {/* Expanded View */}
                            <div>
                                {/* Left panel with artwork */}
                                <div className={"w-4/12"}>
                                    {/* Cover */}
                                </div>
                                {/* Right panel with details */}
                                <div className={"w-8-12"}>
                                    <div>
                                        <p>{ci.game.name}</p>
                                    </div>
                                    <div>
                                        <p>{ci.platform.name}</p>
                                    </div>
                                    <div>
                                        <p>{formatCurrency(ci.purchasePrice)}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </section>
        </section>
    );
}

export default CollectionPage;