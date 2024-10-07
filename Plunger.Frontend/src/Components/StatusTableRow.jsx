import {dateFormat, formatCurrency} from "../Utils.js";
import StatusTableRowExpanded from "./StatusTableRowExpanded.jsx";

function StatusTableRow(props) {
    const {gameStatus} = props;

    return <div key={gameStatus.id} className="bg-amber-200">
        <div className={"flex justify-center text-center"}>
            <p className={"basis-4/12"}>{gameStatus.game.name}</p>
            <p className={"basis-1/12"}>{gameStatus.playState}</p>
            <p className={"basis-1/12"}>{gameStatus.timePlayed}</p>
            <p className={"basis-1/12"}>{dateFormat(gameStatus.timeStarted)}</p>
            <p className={"basis-1/12"}>{gameStatus.completed}</p>
            <p className={"basis-1/12"}>{dateFormat(gameStatus.dateCompleted)}</p>
        </div>
        <StatusTableRowExpanded gameStatus={gameStatus}/>
    </div>
}

export default StatusTableRow;