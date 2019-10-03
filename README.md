# ConfigTransformerCore

.NET Core tool for transforming XML config files, based on [Microsoft's Xml
Document Transformation library](https://github.com/aspnet/xdt).

## Examples

Apply transformations from file `web.release.config` onto source file
`web.base.config`, producing output file `web.config`.

```
ConfigTransformerCore --sourceFile web.base.config --transformFile web.release.config --destinationFile web.config
```

Run the same transformation with verbose logging enabled.

```
ConfigTransformerCore -s web.base.config -s web.release.config -d web.config -verbose
```

## Credits

Some unit test code copied, under terms of the Apache 2.0 license, from the
[original source repository](https://github.com/aspnet/xdt). That code is copyright
of Microsoft.

## Copyright

Copyright 2019, Stephen A. Fuqua, except where noted.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use
this file except in compliance with the License. You may obtain a copy of the
License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
CONDITIONS OF ANY KIND, either express or implied. See the License for the
specific language governing permissions and limitations under the License.
