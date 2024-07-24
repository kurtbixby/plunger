import SearchDropDownItem from "./SearchDropDownItem.jsx"; 

function SearchDropDown(props) {
    const { onFocus, onTextChange, onItemSelect, isOpen, searchResults, name, value } = props;
    
    return (
        <>
            <input
                name={name}
                type={"text"}
                value={value}
                onClick={onFocus}
                onChange={(e) => onTextChange(e)}
            />
            <div>
                {isOpen && (
                    <ul>
                        {searchResults.slice(0, 5).map((game) => (
                            <SearchDropDownItem key={game.id} text={game.name} callback={() => onItemSelect(game)}/>
                        ))}
                    </ul>
                )}
            </div>
        </>
    )
}

export default SearchDropDown;