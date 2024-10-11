import {useParams} from "react-router-dom";
import StatusListTable from "../Widgets/StatusListTable.jsx";
import {useQuery} from "@tanstack/react-query";
import {queryKeyConstants} from "../Hooks/queryKeyConstants.js";
import fetchGameStatuses from "../Hooks/fetchGameStatuses.js";

function StatusListPage() {
    const { userName: username } = useParams();

    const results = useQuery({
        queryKey: [queryKeyConstants.gameStatusView, {username}],
        queryFn: fetchGameStatuses
    });

    const statusResults = results?.data ?? [];
    
    return <section>
        <div>
            <h2>{username + 's Game Statuses'}</h2>
        </div>
        <section className={"flex justify-center"}>
            <StatusListTable gameStatuses={statusResults}/>
        </section>
    </section>
}

export default StatusListPage;