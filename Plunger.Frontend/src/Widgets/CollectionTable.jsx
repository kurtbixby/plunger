import CollectionTableRow from "../Components/CollectionTableRow.jsx";

function CollectionTable(props) {
    const {collectionEntries} = props;

    return <div className={"basis-11/12"}>
        <div className={"flex justify-center text-center"}>
            <p className={"basis-4/12"}>Game</p>
            <p className={"basis-2/12"}>Platform</p>
            <p className={"basis-1/12"}>Purchase Price</p>
            <p className={"basis-1/12"}>Physicality</p>
            <p className={"basis-2/12"}>Region</p>
            <p className={"basis-1/12"}>Date Acquired</p>
            <p className={"basis-1/12"}>Date Added</p>
        </div>
        {collectionEntries?.length > 0 && collectionEntries?.map(ce => <CollectionTableRow key={ce.id} collectionEntry={ce}/>)}
    </div>
}

export default CollectionTable;