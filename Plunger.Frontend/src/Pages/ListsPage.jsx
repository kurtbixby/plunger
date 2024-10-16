import { useParams } from "react-router-dom";

function ListsPage() {
  const { userName } = useParams();

  return (
    <section>
      <div>
        <h2>{userName + "s Lists"}</h2>
      </div>
    </section>
  );
}

export default ListsPage;
