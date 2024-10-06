import {useState} from "react";
import SmallRowElement from "../Components/SmallRowElement.jsx";
import {dateFormat} from "../Utils.js";
import WrappedSelect from "../Components/WrappedSelect.jsx";
import UIConstants from "../UIConstants.js";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import modifyGameStatus from "../Hooks/modifyGameStatus.js";
import {queryKeyConstants} from "../Hooks/queryKeyConstants.js";

function GameStatusWidget(props) {
    const { game: { game, status} } = props;

    const [currentHours, setCurrentHours] = useState(status.timePlayed ?? 0);

    const [playStatus, setPlayStatus] = useState(status.playState ?? 0);

    const queryClient = useQueryClient();

    const mutation = useMutation({
        mutationFn: (editRequest) => {
            return modifyGameStatus(status.userId, game.id, editRequest);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({queryKey: [queryKeyConstants.homePageLists]});
        }
    });

    function changeNumericTextField(text) {
        const unwantedFilter = /[^.\d]/;
        let filteredText = text.replace(unwantedFilter, '');
        let decimalIdx = filteredText.indexOf('.');
        if (decimalIdx !== -1) {
            filteredText = filteredText.replaceAll('.', '');
            filteredText = filteredText.substring(0, decimalIdx) + '.' + filteredText.substring(decimalIdx);
        }
        if (!isNaN(filteredText)) {
            if (decimalIdx === -1) {
                return filteredText; // No decimal, is an integer
            }

            return filteredText.substring(0, Math.min(filteredText.length, decimalIdx + 1 + 2));
        }
        else {
            return text;
        }
    }

    function isPartialDecimal(numberText) {
        if (isNaN(numberText)) {
            return false;
        }

        return numberText.split('.').length === 2;
    }

    async function saveChanges(event) {
        event.preventDefault();

        const editGameRequest = {
            timestamp: new Date(Date.now()).toISOString(),
            playstate: playStatus,
            timeplayed: currentHours
        };

        await mutation.mutateAsync(editGameRequest);
    }
    
    return <div className="flex flex-row bg-fuchsia-400 currentStatus">
        <SmallRowElement label="Date Started" body={dateFormat(status.dateStarted)} />
        <form className="flex grow" onSubmit={saveChanges}>
            <div className="flex grow flex-col content-start items-center">
                <label className="text-center" htmlFor="currentHours">Current Status</label>
                <WrappedSelect name="status" styles="text-center" contents={UIConstants.PlayStates.list} value={playStatus} onSelect={e => setPlayStatus(e.target.value)} />
            </div>
            <div className="flex grow flex-col content-start items-center">
    
                <label className="text-center" htmlFor="currentHours">Hours Played</label>
                <input
                    className="w-20 text-center"
                    name="currentHours"
                    value={currentHours}
                    onChange={(e) => {
                        let newValue = changeNumericTextField(e.target.value);
                        if (e.target.value === '') {
                            setCurrentHours('')
                        } else if (String(newValue) === e.target.value) {
                            setCurrentHours(newValue)
                        } else if (isPartialDecimal(newValue)) {
                            setCurrentHours(newValue)
                        }
                    }
                    }
                />
            </div>
            <button className="grow" type="submit">Save Changes</button>
        </form>
    </div>

}

export default GameStatusWidget;    