import {dateFormat, formatCurrency} from "../Utils.js";
import StatusTableRowExpanded from "./StatusTableRowExpanded.jsx";
import {useState} from "react";
import Enums from "../Enums.js";

function StatusTableRow(props) {
    const {gameStatus} = props;
    const [isExpanded, setIsExpanded] = useState(false);
    
    function toggleExpanded() {
        setIsExpanded(!isExpanded);
    }

    return <div key={gameStatus.id} className={(isExpanded ? " mb-1" : "")}>
        <div className={"border border-solid border-slate-600 flex justify-center text-center bg-amber-200"} onClick={toggleExpanded}>
            <p className={"basis-4/12"}>{gameStatus.game.name}</p>
            <p className={"basis-1/12"}>{Enums.PlayStates[gameStatus.playState]}</p>
            <p className={"basis-1/12"}>{gameStatus.timePlayed}</p>
            <p className={"basis-1/12"}>{dateFormat(gameStatus.timeStarted)}</p>
            <p className={"basis-1/12"}>{gameStatus.completed ? "Yes" : "No"}</p>
            <p className={"basis-1/12"}>{gameStatus.dateCompleted ? dateFormat(gameStatus.dateCompleted) : "N/A"}</p>
        </div>
        {isExpanded && <StatusTableRowExpanded gameStatus={gameStatus}/>}
    </div>
}

export default StatusTableRow;