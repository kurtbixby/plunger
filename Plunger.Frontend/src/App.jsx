import { createRoot } from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import TopNavigation from "./Widgets/TopNavigation";
import ProfilePage from "./Pages/ProfilePage";
import CollectionPage from "./Pages/CollectionPage";
import { CurrentUserProvider } from "./CurrentUserProvider.jsx";
import StatusListPage from "./Pages/StatusListPage.jsx";
import {ChakraProvider} from "@chakra-ui/react";
import LandingPage from "./Pages/LandingPage.jsx";
import LogoutPage from "./Pages/LogoutPage.jsx";
import NavBar from "./Widgets/NavBar.jsx";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: Infinity,
      cacheTime: Infinity,
    },
  },
});

function App() {
  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <CurrentUserProvider>
          <ChakraProvider>
            <NavBar title="Plunger" />
            <Routes>
              <Route path="/" element={<LandingPage />} />
              <Route path="/:userName" element={<ProfilePage />} />
              <Route path="/:userName/collection" element={<CollectionPage />} />
              <Route path="/:userName/gamestates" element={<StatusListPage />} />
              <Route path="/logout" element={<LogoutPage />} />
              {/*<Route path="/:userName/lists" element={<ListsPage />} />*/}
            </Routes>
          </ChakraProvider>
        </CurrentUserProvider>
      </QueryClientProvider>
    </BrowserRouter>
  );
}

const container = document.getElementById("root");
const root = createRoot(container);
root.render(<App />);
