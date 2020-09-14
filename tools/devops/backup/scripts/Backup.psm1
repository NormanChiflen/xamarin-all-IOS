<#
    .SYNOPSIS
        Get all the remote branches in the repo.
    .DESCRIPTION
        Returns an array with all the remote branches of the reopistory.
#>
function Get-AllRemoteBranches 
{
    [CmdletBinding()]
    param ()

    $workingDir = Get-Location
    # execute git, get the output and return all branhces in the arraa
    $result = New-Object System.Collections.Generic.List[string]

    $pinfo = New-Object System.Diagnostics.ProcessStartInfo
    $pinfo.FileName = "git"
    $pinfo.RedirectStandardOutput = $true
    $pinfo.UseShellExecute = $false
    $pinfo.Arguments = "branch -r"
    $pinfo.WorkingDirectory = $workingDir

    $p = New-Object System.Diagnostics.Process
    $p.StartInfo = $pinfo
    $p.Start() | Out-Null
    $rv = $p.StandardOutput.ReadToEnd()
    $p.WaitForExit()
    $rv = $rv.Split([System.Environment]::NewLine)
    # data is dirty we need to clean the array, trim strings and remove those branches already trached
    foreach ($remote in $rv) {
        if (-not ($remote.Contains("->"))) # branch is not tracked
        {
            $clean = $remote.Trim()
            if (-not [System.String]::IsNullOrEmpty($clean))
            {
                $_ = $result.Add($clean)
            }
        }
    }
    return $result
}

<#
    .SYNOPSIS
        Creates a bundle backup of a given repository.
    .DESCRIPTION
        Performs a checkout of a given reopsitory, fetches all the
        remotes branches and creates a bundle file.
#>
function New-Backup 
{
    param
    (
        [Parameter(Mandatory)]
        [String]
        $Repository,

        [Parameter(Mandatory)]
        [String]
        $Url
    )

    # in order to perform a backup we have to perform the following steps:
    # 1. Checkout the dir.
    # 2. Fetch all the remote branches.
    # 3. Pull all the branhces
    # 4. Create the bundle

    Write-Host "Cloning into $Url $Repository"
    git clone $Url $Repository
    # should have the dir present
    Set-Location -Path $Repository
    $remoteBranches = Get-AllRemoteBranches
    for ( $index = 0; $index -lt $remoteBranches.Count; $index++) {
        $remote = $remoteBranches[$index]
        $local = $remote -replace "origin/",""
        git branch --track $local $remote
    }
    git fetch --all
    git pull --all

    # got all the data, create the bundle
    Write-Host "All remote branches are tracked."
    $path = [System.IO.Path]::Combine($path, "$Repository-bundle")
    Write-Host "Creating bundle backup to path $path"

    git bundle create $path --all
    # get out of the repo and where we started
    Set-Location -Path ..
}

<#
    .SYNOPSIS
        Backup all the repos the xamarin-macios team is interested in.
#>
function New-XamarinMaciosBackup
{
    $repositories=@(
        @{repo="xamarin-macios";url="git@github.com:xamarin/xamarin-macios.git"},
        @{repo="maccore";url="git@github.com:xamarin/maccore.git"},
        @{repo="SubmissionSamples";url="git@github.com:xamarin/SubmissionSamples.git"},
        @{repo="XmlDocSync";url="git@github.com:xamarin/XmlDocSync.git"},
        @{repo="ios-api-docs";url="git@github.com:xamarin/ios-api-docs.git"},
        @{repo="mac-api-docs";url="git@github.com:xamarin/mac-api-docs.git"},
        @{repo="xamarin-analysis";url="git@github.com:xamarin/xamarin-analysis.git"}
    )

    foreach ($repoInfo in $repositories) {
        $repo=$repoInfo["repo"]
        $url=$repoInfo["url"]
        Write-Host "Create backup for $repo at $url"
        New-Backup -Repository $repo -Url $url 
    }
}

Export-ModuleMember -Function Get-AllRemoteBranches  
Export-ModuleMember -Function New-Backup
Export-ModuleMember -Function New-XamarinMaciosBackup