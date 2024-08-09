function SmallList({ list }) {
  console.log("List", list);
  return (
    <div className="bg-lime-300">
      <header>
        <h2>{list.title}</h2>
        {console.log(list)}
        {list.entries.map((listEntry) => (
          <div key={listEntry.id} className="listItem">
            {listEntry.name}
          </div>
        ))}
      </header>
    </div>
  );
}

export default SmallList;
