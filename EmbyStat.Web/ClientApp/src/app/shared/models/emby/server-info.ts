import { Drive } from './drive';

export class ServerInfo {
  systemUpdateLevel: number;
  operatingSystemDisplayName: string;
  hasPendingRestart: boolean;
  isShuttingDown: boolean;
  supportsLibraryMonitor: boolean;
  webSocketPortNumber: string;
  canSelfRestart: boolean;
  canSelfUpdate: boolean;
  canLaunchWebBrowser: boolean;
  programDataPath: string;
  itemsByNamePath: string;
  cachePath: string;
  logPath: string;
  internalMetadataPath: string;
  transcodingTempPath: string;
  httpServerPortNumber: number;
  supportsHttps: boolean;
  httpsPortNumber: string;
  hasUpdateAvailable: boolean;
  supportsAutoRunAtStartup: boolean;
  hardwareAccelerationRequiresPremiere: boolean;
  localAddress: string;
  wanAddress: string;
  serverName: string;
  version: string;
  operatingSystem: string;
  drives: Drive[];
}
