import {dateFormat, formatCurrency} from "../Utils.js";
import Enums from "../Enums.js";
import CollectionTableRowExpanded from "./CollectionTableRowExpanded.jsx";

function CollectionTableRow(props) {
    const {collectionEntry} = props;

    return <div key={collectionEntry.id} className="bg-amber-200">
        <div className={"flex justify-center text-center"}>
            <p className={"basis-4/12"}>{collectionEntry.game.name}</p>
            <p className={"basis-2/12"}>{collectionEntry.platform.name}</p>
            <p className={"basis-1/12"}>{formatCurrency(collectionEntry.purchasePrice)}</p>
            <p className={"basis-1/12"}>{Enums.Physicality[collectionEntry.physicality]}</p>
            <p className={"basis-2/12"}>{collectionEntry.region.name}</p>
            <p className={"basis-1/12"}>{dateFormat(collectionEntry.timeAcquired)}</p>
            <p className={"basis-1/12"}>{dateFormat(collectionEntry.timeAdded)}</p>
        </div>
        <CollectionTableRowExpanded collectionEntry={collectionEntry}/>
    </div>
}

export default CollectionTableRow;