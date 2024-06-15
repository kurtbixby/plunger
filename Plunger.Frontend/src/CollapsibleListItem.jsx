function CollapsibleListItem({ children, title, id, isOpen, toggleItem }) {
  return (
    <div>
      <button className="collapsibleHeader" onClick={() => toggleItem(id)}>
        <h3>{title}</h3>
      </button>
      <div className={isOpen ? "" : "h-0 invisible"}>{children}</div>
    </div>
  );
}

export default CollapsibleListItem;
