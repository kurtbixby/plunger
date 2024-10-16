import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import APICalls from "../APICalls.js";
import CollectionTable from "../Widgets/CollectionTable.jsx";
import { useQuery } from "@tanstack/react-query";
import fetchCollectionEntries from "../Hooks/fetchCollectionEntries.js";
import { queryKeyConstants } from "../Hooks/queryKeyConstants.js";

function CollectionPage() {
  const { userName: username } = useParams();

  const results = useQuery({
    queryKey: [queryKeyConstants.collectionView, { username }],
    queryFn: fetchCollectionEntries,
  });

  const collectionResults = results?.data?.games ?? [];

  return (
    <section>
      <div>
        <h2>{username + "s Collection"}</h2>
      </div>
      <section className={"flex justify-center"}>
        {collectionResults && (
          <CollectionTable collectionEntries={collectionResults} />
        )}
      </section>
    </section>
  );
}

export default CollectionPage;
