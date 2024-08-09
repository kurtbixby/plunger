import { useQuery } from "@tanstack/react-query";
import fetchHomePageLists from "../Hooks/fetchHomePageLists";
import SmallList from "../Components/SmallList";
import { useContext } from "react";
import CurrentUserContext from "../CurrentUserContext";
import AddGameWidget from "../Widgets/AddGameWidget";
import NowPlayingWidget from "../Widgets/NowPlayingWidget";

function HomePage() {
  const [currentUser] = useContext(CurrentUserContext);

  const results = useQuery({
    queryKey: ["homePageLists", currentUser],
    queryFn: fetchHomePageLists,
  });
  const lists = results?.data ?? [];

  return (
    <div className="container lg:mx-auto">
      <div className="grid grid-cols-2 gap-3">
        <AddGameWidget />
        <NowPlayingWidget />
      </div>
      <div className="grid grid-cols-2 gap-3">
        {console.log(lists)}
        {!lists.length ? (
          <h1>No Lists</h1>
        ) : (
          lists.map((list) => <SmallList key={list.id} list={list} />)
        )}
      </div>
    </div>
  );
}

export default HomePage;
