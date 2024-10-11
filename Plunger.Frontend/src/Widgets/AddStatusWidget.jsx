import {useState} from "react";
import WrappedSelect from "../Components/WrappedSelect.jsx";
import UIConstants from "../UIConstants.js";
import Enums from "../Enums.js";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import {queryKeyConstants} from "../Hooks/queryKeyConstants.js";
import createGameStatus from "../Hooks/createGameStatus.js";
import {useCurrentUser} from "../CurrentUserProvider.jsx";
import TimeInput from "../Components/TimeInput.jsx";

function AddStatusWidget(props) {
    const { game } = props;

    const [currentHours, setCurrentHours] = useState("00:00:00");

    const [playState, setPlayState] = useState(0);

    const { state: { user: currentUser } } = useCurrentUser();

    const queryClient = useQueryClient();

    const mutation = useMutation({
        mutationFn: (createRequest) => {
            return createGameStatus(currentUser.id, createRequest);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({queryKey: [queryKeyConstants.homePageLists]});
        }
    });
    
    async function saveChanges(event) {
        event.preventDefault();
        
        const request = { gameid: game.id, completed: +playState === Enums.PlayStates.Completed, playstate: +playState, timeplayed: currentHours }
        console.log(request);
        
        await mutation.mutateAsync(request);
    }
    
    return <div className="flex flex-row bg-slate-600">
        <form className="flex grow" onSubmit={saveChanges}>
            <div className="flex grow flex-col content-start items-center">
                <label className="text-center" htmlFor="status">Current Status</label>
                <WrappedSelect name="status" styles="text-center" contents={UIConstants.PlayStates.list}
                               value={playState} onSelect={e => setPlayState(e.target.value)}/>
            </div>
            <div className="flex grow flex-col content-start items-center">
                <label className="text-center" htmlFor="currentHours">Hours Played</label>
                <TimeInput styles="w-20 text-center" name="currentHours" value={currentHours} onChange={setCurrentHours} />
            </div>
            <button className="grow" type="submit">Save Changes</button>
        </form>
    </div>
}

export default AddStatusWidget;