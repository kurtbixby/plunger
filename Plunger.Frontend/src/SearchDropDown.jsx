import SearchDropDownItem from "./SearchDropDownItem.jsx";
import {useRef, useState} from "react";
import {useOnClickOutside} from "usehooks-ts"; 

function SearchDropDown(props) {
    const { onTextChange, onItemSelect, searchResults, name, value } = props;
    
    const [isOpen, setIsOpen] = useState(false);

    const ref = useRef(null);
    useOnClickOutside(ref, closeDropDown);
    
    function openDropDown() {
        setIsOpen(true);
    }
    function closeDropDown() {
        setIsOpen(false);
    }
    
    return (
        <div ref={ref}>
            <input
                name={name}
                type={"text"}
                value={value}
                onClick={openDropDown}
                onChange={e => onTextChange(e.target.value)}
            />
            <div>
                {isOpen && (
                    <ul>
                        {searchResults.slice(0, 5).map((game) => (
                            <SearchDropDownItem key={game.id} text={game.name} callback={() => {
                                onItemSelect(game);
                                closeDropDown();
                            }}/>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    )
}

export default SearchDropDown;