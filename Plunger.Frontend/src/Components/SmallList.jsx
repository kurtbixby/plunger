function SmallList(props) {
    const { list } = props;
  console.log("List", list);
  return (
    <div className="bg-lime-300">
      <header>
        <h2>{list.name}</h2>
        {console.log(list)}
        {list.listEntries.map((listEntry) => (
          <div key={listEntry.id} className="listItem">
            {listEntry.game.name}
          </div>
        ))}
      </header>
    </div>
  );
}

export default SmallList;
