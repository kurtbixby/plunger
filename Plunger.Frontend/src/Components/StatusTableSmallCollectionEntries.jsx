import StatusTableSmallCollectionEntry from "./StatusTableSmallCollectionEntry.jsx";
import {dateFormat} from "../Utils.js";

function StatusTableSmallCollectionEntries(props) {
    const {collectionEntries} = props;
    
    return <div className="border border-solid border-slate-600 border-t-0 bg-teal-300 mx-2 rounded-b-md">
        <div className={"flex place-content-center text-center"}>
            <p className={"basis-2/12"}>Platform</p>
            <p className={"basis-1/12"}>Physicality</p>
            <p className={"basis-1/12"}>Date Acquired</p>
            <p className={"basis-1/12"}>Date Added</p>
            <p className={"basis-1/12"}>Purchase Price</p>
            <p className={"basis-1/12"}>Region</p>
        </div>
        {collectionEntries.map(e => <StatusTableSmallCollectionEntry key={e.id} entry={e}/>)}
    </div>
}

export default StatusTableSmallCollectionEntries;