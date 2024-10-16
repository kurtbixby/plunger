import {dateFormat, formatCurrency} from "../Utils.js";
import Enums from "../Enums.js";

function StatusTableSmallCollectionEntry(props) {
    const {entry} = props;
    
    return <div className="flex place-content-center text-center">
        <p className={"basis-2/12"}>{entry.platform.name}</p>
        <p className={"basis-1/12"}>{Enums.Physicality[entry.physicality]}</p>
        <p className={"basis-1/12"}>{dateFormat(entry.timeAcquired)}</p>
        <p className={"basis-1/12"}>{dateFormat(entry.timeAdded)}</p>
        <p className={"basis-1/12"}>{formatCurrency(entry.purchasePrice)}</p>
        <p className={"basis-1/12"}>{entry.region.name}</p>
    </div>
}

export default StatusTableSmallCollectionEntry;