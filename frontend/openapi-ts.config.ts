import { defineConfig } from "@hey-api/openapi-ts";
import { defaultPlugins } from "@hey-api/openapi-ts";

export default defineConfig({
  input: "../shared/api/Veloci.Web.json",
  output: "src/api/client",
  client: "@hey-api/client-fetch",
  experimentalParser: true,
  plugins: [...defaultPlugins],
});